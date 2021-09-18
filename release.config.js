"use strict";
const fs = require("fs");
var readFile = require("fs").readFileSync;
var resolve = require("path").resolve;

const plugins = require("./github.json");

const transformCommitType = type => {
    const commitTypeMapping = {
        feat: "Features",
        fix: "Bug Fixes",
        perf: "Performance Improvements",
        revert: "Reverts",
        docs: "Documentation",
        style: "Styles",
        refactor: "Code Refactoring",
        test: "Tests",
        build: "Build System",
        ci: "Continuous Integration",
        chore: "Chores",
        default: "Miscellaneous"
    };
    return commitTypeMapping[type] || commitTypeMapping["default"];
};

var labels = {
    BREAK: {
        title: "BREAKING CHANGES",
        collapse: false
    },
    NEW: {
        title: "New Features",
        collapse: false
    },
    FIX: {
        title: "Bug Fixes",
        collapse: false
    },
    DOC: {
        title: "Documentation",
        collapse: true
    },
    OTHER: {
        title: "Others",
        collapse: true
    }
};

var sort = Object.keys(labels);

var customTransform = (commit, context) => {
    const issues = [];

    commit.notes.forEach(note => {
        note.title = `BREAKING CHANGES`;
    });

    commit.type = transformCommitType(commit.type);

    if (commit.scope === "*") {
        commit.scope = "";
    }

    if (typeof commit.hash === `string`) {
        commit.shortHash = commit.hash.substring(0, 7);
    }

    if (typeof commit.subject === `string`) {
        let url = context.repository
            ? `${context.host}/${context.owner}/${context.repository}`
            : context.repoUrl;
        if (url) {
            url = `${url}/issues/`;
            // Issue URLs.
            commit.subject = commit.subject.replace(/#([0-9]+)/g, (_, issue) => {
                issues.push(issue);
                return `[#${issue}](${url}${issue})`;
            });
        }
        if (context.host) {
            // User URLs.
            commit.subject = commit.subject.replace(
                /\B@([a-z0-9](?:-?[a-z0-9/]){0,38})/g,
                (_, username) => {
                    if (username.includes("/")) {
                        return `@${username}`;
                    }

                    return `[@${username}](${context.host}/${username})`;
                }
            );
        }
    }

    // remove references that already appear in the subject
    commit.references = commit.references.filter(reference => {
        if (issues.indexOf(reference.issue) === -1) {
            return true;
        }

        return false;
    });

    return commit;
};

var writerOpts = {
    transform: customTransform,
    groupBy: "type",
    commitGroupsSort: function (a, b) {
        return sort.indexOf(a.title) > sort.indexOf(b.title);
    },
    finalizeContext: function (context) {
        context.commitGroups.forEach(function (group) {
            Object.assign(group, labels[group.title.toUpperCase()]);
        });

        // console.log(context);
        return context;
    },
    commitsSort: ["subject"]
};

writerOpts.mainTemplate = readFile(resolve(__dirname, "SemanticVersioning/template.hbs"), "utf-8");
writerOpts.headerPartial = readFile(resolve(__dirname, "SemanticVersioning/header.hbs"), "utf-8");
writerOpts.commitPartial = readFile(resolve(__dirname, "SemanticVersioning/commit.hbs"), "utf-8");
writerOpts.footerPartial = readFile(resolve(__dirname, "SemanticVersioning/footer.hbs"), "utf-8");


/*var releaseRules = [
    { type: "docs", scope: "README", release: "patch" },
    { type: "refactor", scope: "core-*", release: "minor" },
    { type: "refactor", release: "patch" },
    { type: "build", release: "patch" },
    { type: "ci", release: "patch" },
    { type: "chore", release: "patch" },
    { type: "style", release: "patch" },
    { type: "test", release: "patch" },
    { type: "fix", release: "patch" }
];*/

module.exports = {
    //releaseRules: releaseRules,
    writerOpts: writerOpts,
    ...plugins
};
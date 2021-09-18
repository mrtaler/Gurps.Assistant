module.exports = [
    { type: "docs", scope: "README",        release: "patch" },
    { type: 'refactor', scope: 'core-*',    release: 'minor' },
    { type: 'refactor',                     release: 'patch' },
    { type: "build",                        release: "patch" },
    { type: "ci",                           release: "patch" },
    { type: "chore",                        release: "patch" },
    { type: "style",                        release: "patch" },
    { type: "test",                         release: "patch" },
    { type: "fix",                          release: "patch" }
];
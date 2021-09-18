module.exports = [
    {
        mergePattern: /^Merge pull request #(.*) from .*$/,
        mergeCorrespondence: [
            "id",
            "source"
        ]
    }
];
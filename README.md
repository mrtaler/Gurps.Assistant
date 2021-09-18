# BoardGameAssistant
---
[![semantic-release](https://img.shields.io/badge/%20%20%F0%9F%93%A6%F0%9F%9A%80-semantic--release-e10079.svg)](https://github.com/semantic-release/semantic-release)
---

Init readme



https://github.com/GitTools/GitReleaseManager
https://github.com/StefH/GitHubReleaseNotes

 git commit -m "feat: ajuste A"
 
 feat: initial commit (for new branches)
 
 fix: change file

---
### mongo db
docker run -d  -e MONGO_INITDB_ROOT_USERNAME=admin -e MONGO_INITDB_ROOT_PASSWORD=password -e MONGO_INITDB_DATABASE=TestMongoDB   -p 27017:27017   --name some-mongo   mongo:latest
mongo --username admin --password password


yarn add semantic-release
npx semantic-release-cli setup

|npm| install| semantic-release-cli||
|||@semantic-release/release-notes-generator||
|||@semantic-release/github||
|||@semantic-release/git||
|||@semantic-release/commit-analyzer||
|||@semantic-release/changelog||
|||@commitlint/cli||
|||@commitlint/config-conventional|echo "module.exports = {extends: ['@commitlint/config-conventional']}" > commitlint.config.js|
||npm install husky --save-dev|npx husky install|npx husky add .husky/commit-msg 'npx --no-install commitlint --edit "$1"'|


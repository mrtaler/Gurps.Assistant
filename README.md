# GurpsAssistant
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
|

|npm install husky --save-dev
|npx husky install
|npm set-script prepare "husky install"
|yarn husky add .husky/pre-commit "npm test"
|(https://typicode.github.io/husky/#/?id=automatic-recommended)

npm install --save-dev @commitlint/cli
npm install --save-dev @commitlint/config-conventional
npm install --save-dev @commitlint/prompt-cli

docker run -p 8080:8080 ravendb/ravendb:latest


 raven2:
        container_name: raven2
        image: ravendb/ravendb
        ports:d
            - 8082:8080
            - 38889:38888
        environment:
            - RAVEN_Security_UnsecuredAccessAllowed=PrivateNetwork
            - RAVEN_Setup_Mode=None
            - RAVEN_License_Eula_Accepted=true
            - "RAVEN_ServerUrl=http://0.0.0.0:8080"
            - "RAVEN_PublicServerUrl=http://localhost:8082"
            - "RAVEN_ServerUrl_Tcp=tcp://0.0.0.0:38888"
            - "RAVEN_PublicServerUrl_Tcp=tcp://localhost:38889"


            BIND_PORT=8080
BIND_TCP_PORT=38888
SECURITY_UNSECURED_ACCESS_ALLOW="PublicNetwork"
ARGS="--Setup.Mode=Unsecured"
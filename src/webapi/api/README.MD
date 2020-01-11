# moneyboard api

## Import 

### Prepare import
curl --insecure -X POST https://localhost:5001/import/prepare?accountId=4 -F file=@test.qif > import.json

### Upload to account

marche pas. a voir

curl --insecure -X POST https://localhost:5001/import/uploadtoaccount -H "Content-Type:application/json;charset=utf-8" -d @import.json
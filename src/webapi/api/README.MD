# Tests

### Tester accès get

curl -G -v http://localhost:5000/transactions_view/account/1

### Tester import simple (avec fichier en tant que body)

curl -X POST -F "files=@../data/sample.qif" http://localhost:5000/import/prepare


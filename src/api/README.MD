# Tests

### Tester accès get

curl -G -v http://localhost:5000/transactions_view/account/1

### Tester import simple (avec fichier en tant que body)

curl -X POST -F "file=/data/sample.qif" http://localhost:5000/import

TODO : ne fonctionne pas encore - voir pouquoi le fichier n'arrive pas dans foreach(var file in  Request.Form.Files) 


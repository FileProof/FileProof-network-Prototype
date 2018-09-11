# FileProof-network-Prototype

Fileproof project is a prototype, designed as a proof of concept and aimed to demonstrate the validation/verification of the documents, stored as a single graph with blockchain anchoring.

The application has three screens:

- Validate 

Here you can upload the data. 

The form is divided into two parts:
Data can be presented either as a plain text or a file.

After the upload system calculates the hash of data and saves it as a document. 
No data saved in the system, only the hash!

Validator field is optional, it is used in case the document is targeted to a certain validator. If the validator id is set during upload, later he could filter the documents with his id.

- Verify

Once the document, holding the data is created, it can be used to verify if the certain instance of the data equals previously validated one.

Match result is indicated with light green or light red background.

- Documents

This form is visible only to the authorized user.
The authorization in prototype mode is done via any previously validated document Id. This could be, for instance, the uploaded legal document or any other validated data.
After logging in user has access to confirm the validation of uploaded documents. All confirmed during the session documents will refer to the current Id used to log in. 

Documents list contains all the uploaded documents. After the upload, each document has the Data hash and the Id. Document Id, in turn, is the hash of the whole document.

The validator checks the list, and confirms the validation of each document, pressing the green button to the right. 
Then the document hash (the id) is passed to the ethereum network, and gains the storage info: 
- block number
- transaction code
- timestamp

These three attributes are confirming the document validation status and can be checked against independent sources, like etherscan.

Note: you can use **AFIS.Reader.Service** which is a Microsoft Windows service to get WSQ from fingerprints on the client machine.
https://github.com/bjdavey/AFIS.Reader  

## AFIS.Server
Verify method:
Fingerprint matching, also known as 1:1 (one-to-one), using WSQ (Wavelet Scalar Quantization) as an input parameter for each one.  

•	URL:
/AFIS/Verify

•	Full URL:
https://localhost:44395/AFIS/Verify

•	Method:
POST

•	Content-Type:
"multipart/form-data"

•	URL Parameters:
none

•	Data Parameters (Form Data):
1) Wsq1: WSQ of the first fingerprint image	"\/6D\/pAA6CQcACTLTJc0ACuDz………”  
2) Wsq2: WSQ of the second fingerprint image	"\/6D\/pAA6CQcACTLTJc0ACuD………”  
*The two Parameters are always required. 

•	Success Response:  
status: true  
verificationResult:  true | false  
score* : (>= 0)   
*If the score is above zero, the two fingerprints match and similarity score reflects accuracy of the match.  

•	Error Response:  
status: false  
error: (Error Message)  
innerException: (Details of the Error message)

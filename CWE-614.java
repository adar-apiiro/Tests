import javax.servlet.http.Cookie;

String ACCOUNT_ID = "your_account_id";
String acctID = "your_acct_id_value";

Cookie c = new Cookie(ACCOUNT_ID, acctID);
response.addCookie(c);

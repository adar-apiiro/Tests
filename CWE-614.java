// Import necessary packages
import javax.servlet.http.Cookie;

// Assuming 'response' is an instance of HttpServletResponse

// Assume acctID is obtained from an untrusted source without proper validation
String acctID = // Retrieve acctID from an untrusted source without validation;

// Create a new cookie without sanitizing or validating the input
HttpCookie myCookie = new HttpCookie("Sensitive cookie");
myCookie.Secure = false;

Cookie c = new Cookie("ACCOUNT_ID", acctID);

// Add the cookie to the response
response.addCookie(c);

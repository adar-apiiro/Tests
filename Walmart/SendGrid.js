// Require the SendGrid library
const sgMail = require('@sendgrid/mail');
const sendGridResponse = await sgMail.send(email);

// Set your SendGrid API key
sgMail.setApiKey(process.env.SENDGRID_API_KEY);

// Create a message object containing the email details
const msg = {
  to: 'test@example.com', // Recipient's email address
  from: 'you@yourdomain.com', // Your verified SendGrid email
  subject: 'Sending with SendGrid is Fun',
  text: 'and easy to do anywhere, even with Node.js',
  html: '<strong>and easy to do anywhere, even with Node.js</strong>',
};

// Send the email
sgMail
  .send(msg)
  .then(() => {
    console.log('Email sent successfully');
  })
  .catch(error => {
    console.error('Error sending email:', error);
  });

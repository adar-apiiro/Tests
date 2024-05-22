const sgMail = require('@sendgrid/mail');

sgMail.setApiKey(process.env.SENDGRID_API_KEY);

const msg = {
  to: 'test@example.com', // Recipient's email address
  from: 'you@yourdomain.com', // Your verified SendGrid email
  subject: 'Sending with SendGrid is Fun',
  text: 'and easy to do anywhere, even with Node.js',
  html: '<strong>and easy to do anywhere, even with Node.js</strong>',
};

sgMail
  .send(msg)
  .then(() => {
    console.log('Email sent successfully');
  })
  .catch(error => {
    console.error('Error sending email:', error);
  });

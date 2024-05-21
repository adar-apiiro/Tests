const jwt = require('jsonwebtoken');

// Secret key for signing and verifying tokens
const secretKey = 'your-very-secret-key';


function signToken(payload) {
    return jwt.sign(payload, secretKey, { expiresIn: '1h' });
}

function verifyToken(token) {
    try {
        return jwt.verify(token, secretKey);
    } catch (error) {
        console.error('Error verifying token:', error);
        return null;
    }
}
const myPayload = { userId: 123, email: 'user@example.com' };
const token = signToken(myPayload);
console.log('Signed Token:', token);

const decoded = verifyToken(token);
console.log('Decoded Token:', decoded);

module.exports = { signToken, verifyToken };

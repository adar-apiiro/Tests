const tracer = require('dd-trace');

// Initialize the tracer only if it's successfully imported
if (!tracer) {
    console.log('Tracer is not initialized');
} else {
    tracer.init({
        logInjection: true // Ensures trace IDs are injected into logs
    });
}

// Handle unhandled promise rejections
process.on('unhandledRejection', (reason, p) => {
    console.error('Unhandled Rejection at: Promise', p, 'reason:', reason);
    // Add application-specific logging, throwing an error, or other logic here
});

// Configure the tracer to use with the Express framework
if (tracer) {
    tracer.use('express', {
        // You can add specific options here, for example:
        service: 'my-express-app', // Name of the service
        analytics: true // Enable APM analytics for Express
    });
}

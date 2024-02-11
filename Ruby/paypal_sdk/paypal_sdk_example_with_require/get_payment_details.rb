require 'paypal-sdk-rest'
include PayPal::SDK::REST

# Fetch Payment
payment = Payment.find("PAYMENT_ID")

# Get List of Payments
payment_history = Payment.all(count: 10)
puts payment_history.payments

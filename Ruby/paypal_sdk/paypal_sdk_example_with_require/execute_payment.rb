require 'paypal-sdk-rest'
include PayPal::SDK::REST

payment = Payment.find("PAYMENT_ID")

if payment.execute(payer_id: "PAYER_ID")
  puts "Payment executed successfully."
else
  puts payment.error
end

require 'paypal-sdk-rest'
include PayPal::SDK::REST

PayPal::SDK::REST.set_config(
  mode: "sandbox",
  client_id: "your_client_id",
  client_secret: "your_client_secret")

@payment = Payment.new({
  intent: "sale",
  payer: {
    payment_method: "paypal" },
  redirect_urls: {
    return_url: "http://localhost:3000/payment/execute",
    cancel_url: "http://localhost:3000/" },
  transactions: [{
    item_list: {
      items: [{
        name: "item",
        sku: "item",
        price: "5",
        currency: "USD",
        quantity: 1 }]},
    amount: {
      total: "5",
      currency: "USD" },
    description: "This is the payment transaction description." }]})
    
if @payment.create
  puts "Payment created with ID: #{@payment.id}"
else
  puts @payment.error
end

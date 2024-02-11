require 'stripe'

# Set your secret key: remember to change this to your live secret key in production
# See your keys here: https://dashboard.stripe.com/account/apikeys
Stripe.api_key = "sk_test_4eC39HqLyjWDarjtT1zdp7dc"

begin
  charge = Stripe::Charge.create({
    amount: 2000, # Amount in cents ($20.00)
    currency: 'usd',
    source: 'tok_visa', # Obtain this token with Stripe.js or Stripe Elements
    description: 'My first payment',
  })
  puts "Charge successfully created: #{charge.id}"
rescue Stripe::StripeError => e
  puts "Error creating charge: #{e.message}"
end

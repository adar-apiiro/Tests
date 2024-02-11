# Stripe payment integration example
require 'stripe'

Stripe.api_key = 'your_stripe_secret_key'

begin
  charge = Stripe::Charge.create({
    amount: 2000, # $20.00
    currency: 'usd',
    source: 'tok_visa', # Replace with a real source ID
    description: 'My first payment',
  })
  puts "Charge succeeded: \#{charge.id}"
rescue Stripe::StripeError => e
  puts "Charge failed: \#{e.message}"
end

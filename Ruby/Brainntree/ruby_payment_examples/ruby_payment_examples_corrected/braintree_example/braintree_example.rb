# Braintree payment integration example
require 'braintree'

Braintree::Configuration.environment = :sandbox
Braintree::Configuration.merchant_id = 'your_merchant_id'
Braintree::Configuration.public_key = 'your_public_key'
Braintree::Configuration.private_key = 'your_private_key'

result = Braintree::Transaction.sale(
  amount: "10.00",
  payment_method_nonce: 'fake-valid-nonce',
  options: {
    submit_for_settlement: true
  }
)

if result.success?
  puts "Transaction successful! Transaction ID: \#{result.transaction.id}"
else
  puts "Transaction failed: \#{result.message}"
end

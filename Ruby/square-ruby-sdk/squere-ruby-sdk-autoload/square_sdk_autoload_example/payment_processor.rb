# payment_processor.rb
require 'square'

class PaymentProcessor
  # Initialize the Square client
  Square::Client.new(access_token: 'your_access_token', environment: 'sandbox')

  def self.init_payment
    puts 'PaymentProcessor has been autoloaded and initialized.'
    # Add code here to create a payment using the Square SDK
  end
end

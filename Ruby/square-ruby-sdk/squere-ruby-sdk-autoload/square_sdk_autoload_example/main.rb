# main.rb
puts 'Square SDK Autoload Example'

# Demonstrating the use of autoload to load the PaymentProcessor
autoload :PaymentProcessor, './payment_processor'

# Simulate calling a method from the PaymentProcessor which will autoload the class
puts 'Initializing payment...'
PaymentProcessor.init_payment

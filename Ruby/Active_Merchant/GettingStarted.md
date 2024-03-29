# Getting Started with Active Merchant

Before getting started using Active Merchant, a bit of terminology is needed.

In order to process credit card payments, your application needs to interface with a payment
gateway. In Active Merchant, these are represented as subclasses of `ActiveMerchant::Billing::Gateway`.

## Gateway Operations

A typical interaction consists of the application obtaining the necessary credit card credentials
(card number, expiry date, etc.) and asking the gateway to *authorize* the required amount on the
card holder's credit card.

If the authorization is successful, the funds are available and you can ask the gateway to *capture*
them to your account. If the capture is completed, the payment has been made.

When combined into a single operation, this is called a *purchase*.

All of these operations are performed on an instance of a Gateway subclass:

```ruby
gateway = SomeGateway.new

# Amounts are always specified in cents, so $10.00 is 1000 cents
response = gateway.purchase(1000, credit_card)
```

All three `#authorize`, `#capture` and `#purchase` methods return a `ActiveMerchant::Billing::Response` instance.
This object contains the details of the operation, most notably whether it was successful.

```ruby
if response.success?
  puts "Payment complete!"
else
  puts "Payment failed: #{response.message}"
end
```

## Handling Credit Cards

In Active Merchant, credit cards are represented by instances of `ActiveMerchant::Billing::CreditCard`.
Instantiating such an object is simple:

```ruby
credit_card = ActiveMerchant::Billing::CreditCard.new(
  :first_name         => 'Steve',
  :last_name          => 'Smith',
  :month              => '9',
  :year               => '2022',
  :brand              => 'visa',
  :number             => '4242424242424242',
  :verification_value => '424')
```

Most often, though, you'll be using user-supplied data. In a typical Rails controller:

```ruby
credit_card = ActiveMerchant::Billing::CreditCard.new(params[:credit_card])
```

### Validation

While the above attributes are always required for a `CreditCard` to be valid, some gateways also
require a *verification value*, e.g. a CVV code, to be given.

Validating a credit card is as simple as calling `CreditCard#valid?`, which
returns `true` only if the credentials are syntactically valid. If there are any errors or omissions,
the `CreditCard#errors` attribute will be non-empty.


### good to know :

Setting Up a Gateway: Depending on which payment gateway you plan to use, you would create an instance of that gateway. Active Merchant supports a wide range of gateways, each of which is a class under ActiveMerchant::Billing.

For example, to use the TrustCommerce gateway, you would do:
```ruby
gateway = ActiveMerchant::Billing::TrustCommerceGateway.new(
    login: 'TestMerchant',
    password: 'password'
)

```

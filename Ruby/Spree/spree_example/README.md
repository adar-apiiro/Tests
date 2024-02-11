# Spree Commerce Payment Example

This Ruby project demonstrates how to use the Spree Commerce framework to create payments.

## Setup

1. Install Bundler if you haven't already:
```bash
gem install bundler
```

2. Install the dependencies by running:
```bash
bundle install
```

3. Setup Spree:
```bash
rails new my_store
cd my_store
echo "gem 'spree', '~> 4.2'" >> Gemfile
echo "gem 'spree_auth_devise', '~> 4.3'" >> Gemfile
echo "gem 'spree_gateway', '~> 3.9'" >> Gemfile
bundle install
rails g spree:install --user_class=Spree::User
rails g spree:auth:install
rails g spree_gateway:install
```

4. Configure your Spree store and payment gateway in `create_payment.rb`.

## Running the Example

This example requires a full Rails environment. The `create_payment.rb` is a placeholder for where to implement payment logic within your Spree Rails application.

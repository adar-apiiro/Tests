# Solidus Payment Example

This Ruby project demonstrates how to integrate and use payments within the Solidus framework.

## Setup

1. Ensure you have Ruby and Rails installed on your system.
2. Install Bundler if you haven't already:
```bash
gem install bundler
```
3. Navigate to the project directory and install the dependencies:
```bash
cd solidus_example
bundle install
```
4. Set up the Solidus store:
```bash
bin/rails g solidus:install
```
5. Start the Rails server:
```bash
bin/rails server
```

## Implementing Payments

This example includes a basic implementation of a PaymentsController to illustrate how payments could be handled. You'll need to further customize this for real-world use.

Please check `app/controllers/payments_controller.rb` for the payment processing example.

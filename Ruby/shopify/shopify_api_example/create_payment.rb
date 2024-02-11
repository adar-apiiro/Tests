# create_payment.rb
require 'shopify_api'
require 'httparty'

# Configure the Shopify API with your credentials
SHOP_NAME = 'your-shop-name'
API_KEY = 'your-api-key'
PASSWORD = 'your-api-password'
VERSION = '2021-07'

ShopifyAPI::Base.site = "https://\#{API_KEY}:\#{PASSWORD}@\#{SHOP_NAME}.myshopify.com/admin/api/\#{VERSION}/"

# Example method to create a payment (adjust according to Shopify's API and your needs)
def create_payment
  # Example: Create a new order with a payment
  order_data = {
    order: {
      line_items: [
        {
          variant_id: 1234567890,
          quantity: 1
        }
      ],
      financial_status: 'paid'
    }
  }

  response = ShopifyAPI::Order.create(order_data)

  if response.persisted?
    puts "Order created with ID: \#{response.id}"
  else
    puts "Failed to create order: \#{response.errors.full_messages.join(", ")}"
  end
end

create_payment

require 'shopify_api'
require 'httparty'

# Replace these with your actual shop name and API credentials
SHOP_NAME = 'your-shop-name'
API_KEY = 'your-api-key'
PASSWORD = 'your-api-password'
API_VERSION = '2021-10'

# Initialize the ShopifyAPI with your shop
ShopifyAPI::Base.site = "https://\#{API_KEY}:\#{PASSWORD}@\#{SHOP_NAME}.myshopify.com/admin/api/\#{API_VERSION}/"

# Example method to create an order
def create_order
  order = ShopifyAPI::Order.new(
    email: "foo@example.com",
    fulfillments: [],
    line_items: [
      {
        variant_id: 123456789,
        quantity: 1
      }
    ],
    financial_status: "pending",
    transactions: [
      {
        kind: "authorization",
        status: "success",
        amount: "10.00"
      }
    ],
    billing_address: {
      first_name: "John",
      last_name: "Doe",
      address1: "123 Billing Street",
      city: "Billtown",
      province: "Billing Province",
      country: "Billing Country",
      zip: "B1234"
    },
    shipping_address: {
      first_name: "John",
      last_name: "Doe",
      address1: "123 Shipping Street",
      city: "Shiptown",
      province: "Shipping Province",
      country: "Shipping Country",
      zip: "S1234"
    }
  )

  if order.save
    puts "Order created successfully: \#{order.id}"
  else
    puts "Failed to create order: \#{order.errors.full_messages.join(', ')}"
  end
end

# Call the method to create an order
create_order

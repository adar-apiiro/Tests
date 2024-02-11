
order = Spree::Order.find_by(number: "R123456789")


if order.present? && order.payment_state != 'paid'
  payment_amount = order.total
  

  payment_method = Spree::PaymentMethod.first

  payment = order.payments.create!(
    amount: payment_amount,
    payment_method_id: payment_method.id,
    state: 'pending' # Set initial payment state
  )

  if payment.process!
    puts "Payment (ID: #{payment.id}) successfully processed for Order #{order.number}"
  else
    puts "Payment processing failed for Order #{order.number}"
  end
else
  puts "Order not found or already paid"
end

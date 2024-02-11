class PaymentsController < ApplicationController
  def create
    # Example payment creation logic
    order = Spree::Order.find_by(number: params[:order_number])
    if order.present? && order.payments.create(payment_params)
      render json: { status: 'Payment created successfully' }, status: :created
    else
      render json: { error: 'Payment creation failed' }, status: :unprocessable_entity
    end
  end

  private

  def payment_params
    params.require(:payment).permit(:amount, :payment_method_id)
    # Ensure these parameters match your payment method and order details
  end
end

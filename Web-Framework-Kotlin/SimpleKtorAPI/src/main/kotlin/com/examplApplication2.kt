import io.ktor.http.*
import io.ktor.server.application.*
import io.ktor.server.response.*
import io.ktor.server.routing.*
import com.example.models.*

// Placeholder data storage
val customerStorage = mutableListOf<Customer>()
val orderStorage = mutableListOf<Order>()

fun Route.customerRouting() {
    route("/customer") {
        get {
            if (customerStorage.isNotEmpty()) {
                call.respond(customerStorage)
            } else {
                call.respondText("No customers found", status = HttpStatusCode.OK)
            }
        }
        get("{id?}") {
            val id = call.parameters["id"]
            val customer = customerStorage.find { it.id == id }
            if (customer != null) {
                call.respond(customer)
            } else {
                call.respondText("Not Found", status = HttpStatusCode.NotFound)
            }
        }
        post {
            // Placeholder for creating a customer
            call.respondText("Customer created", status = HttpStatusCode.Created)
        }
        delete("{id?}") {
            // Placeholder for deleting a customer
            call.respondText("Customer deleted", status = HttpStatusCode.OK)
        }
    }
}

fun Route.listOrdersRoute() {
    get("/order") {
        if (orderStorage.isNotEmpty()) {
            call.respond(orderStorage)
        } else {
            call.respondText("No orders found", status = HttpStatusCode.OK)
        }
    }
}

fun Route.getOrderRoute() {
    get("/order/{id?}") {
        val id = call.parameters["id"] ?: return@get call.respondText("Bad Request", status = HttpStatusCode.BadRequest)
        val order = orderStorage.find { it.number == id }
        if (order != null) {
            call.respond(order)
        } else {
            call.respondText("Not Found", status = HttpStatusCode.NotFound)
        }
    }
}

fun Application.configureRouting() {
    routing {
        customerRouting()
        listOrdersRoute()
        getOrderRoute()
    }
}

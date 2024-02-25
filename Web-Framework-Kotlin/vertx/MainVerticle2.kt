package com.example

import io.vertx.core.AbstractVerticle
import io.vertx.core.Future
import io.vertx.ext.web.Router

class MainVerticle : AbstractVerticle() {

    override fun start(startPromise: Future<Void>) {
        val router = Router.router(vertx)

        router.route("/api/hello").handler { routingContext ->
            val response = routingContext.response()
            response.putHeader("content-type", "application/json")
            response.end("""{"message":"Hello from Vert.x and Kotlin!"}""")
        }

        vertx.createHttpServer().requestHandler(router).listen(8888) { http ->
            if (http.succeeded()) {
                startPromise.complete()
                println("HTTP server started on port 8888")
            } else {
                startPromise.fail(http.cause())
            }
        }
    }
}

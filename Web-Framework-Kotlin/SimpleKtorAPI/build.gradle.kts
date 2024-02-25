plugins {
    kotlin("jvm") version "1.6.10"
    application
}

application {
    mainClass.set("com.example.ApplicationKt")
}

repositories {
    mavenCentral()
}

dependencies {
    implementation("io.ktor:ktor-server-netty:1.6.7")
    implementation("io.ktor:ktor-server-core:1.6.7")
    implementation("ch.qos.logback:logback-classic:1.2.3")
}

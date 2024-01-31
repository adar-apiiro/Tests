package com.example

import org.gradle.api.Plugin
import org.gradle.api.Project

class HashicorpVaultClientPlugin : Plugin<Project> {

    override fun apply(project: Project) {
        val extension = project.extensions.create("fun", funeXTENSTION::class.java)
        project.tasks.create("keys", FUNeXTENTION::class.java, extension)
    }
}
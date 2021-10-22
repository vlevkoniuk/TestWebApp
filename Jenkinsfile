pipeline {
    agent any
    environment {
        DISABLE_AUTH = 'true'
        DB_ENGINE    = 'sqlite'
        GIT_BRANCH = 'master'
    }
    stages {
        stage('Fetch') {
            steps {
                sh 'echo "Fetch Code"'
                sh '''
                    echo "Multiline shell steps works too"
                    ls -lah
                '''
                 git(
                    url: 'https://github.com/vlevkoniuk/TestWebApp.git',
                    credentialsId: 'vlevkoniuk',
                    branch: "${GIT_BRANCH}"
                )    
                sh 'git status'
                sh 'git show'
                sh 'ls -R'
            }
        }
        stage('Build') {
            steps {
                sh 'echo "Build Code"'
                sh 'dotnet --version'
                sh '''
                    pwd
                    cd TestWebApp
                    echo "================== starting the build of TestWebApp =================="
                    dotnet build 
                '''
            }
        }
        stage('Tests') {
            steps {
                sh 'echo "Build Code"'
                sh '''
                    echo ""
                    pwd
                    cd TestWebApp.Tests
                    echo "=========== Building Test Project =========="
                    dotnet add package JUnitTestLogger --version 1.1.0
                    dotnet build
                    
                    echo "=========== Copying Data file ============ "
                    cp ../TestWebApp/Data/Data[1].xml bin/Debug/netcoreapp3.1/Data/Data[1].xml
                    
                    echo "============= Running Tests ================ "
                    mkdir -p din/Debug/TestResult
                    dotnet test --logger "junit;LogFilePath=bin/Debug/TestResult/TestResults.xml"
                '''
            }
        }
        stage('Deploy') {
            steps {
                sh 'echo "Build Code"'
                sh '''
                    echo "Multiline shell steps works too"
                    ls -lah
                '''
                sh 'dotnet --version'
            }
        }
    }
    post {
        always {
            echo 'This will always run'
            sh 'echo "========== Archiving Test Reports =========="'
            junit 'bin/Debug/TestResult/TestResults.xml'
            sh 'pwd'
            dotnet clean
        }
        success {
            echo 'This will run only if successful'
        }
        failure {
            echo 'This will run only if failed'
        }
        unstable {
            echo 'This will run only if the run was marked as unstable'
        }
        changed {
            echo 'This will run only if the state of the Pipeline has changed'
            echo 'For example, if the Pipeline was previously failing but is now successful'
        }
    }
}
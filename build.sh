#!/bin/sh -e
CGO_ENABLED=0 GOOS=windows GOARCH=amd64 go build -o gsutil.exe main.go
CGO_ENABLED=0 GOOS=linux GOARCH=amd64 go build -o gsutil main.go
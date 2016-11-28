package main

import (
	"context"
	"fmt"
	"os"

	"golang.org/x/oauth2/google"

	storage "google.golang.org/api/storage/v1"
	kingpin "gopkg.in/alecthomas/kingpin.v2"
)

func newStorageClient(scope string) (*storage.Service, error) {
	dc, err := google.DefaultClient(context.Background(), scope)
	if err != nil {
		return nil, err
	}
	gs, err := storage.New(dc)
	if err != nil {
		return nil, err
	}
	return gs, nil
}

func printBuckets(gs *storage.Service, project string) error {
	res, err := gs.Buckets.List(project).Do()
	if err != nil {
		return err
	}
	for _, item := range res.Items {
		fmt.Println(item.Id)
	}
	return nil
}

var (
	gsutil    = kingpin.New("gsutil", "Google Storage utility app")
	ls        = gsutil.Command("ls", "list the buckets")
	lsProject = ls.Flag("project", "Google Cloud project").Required().Short('p').String()
)

func main() {
	switch kingpin.MustParse(gsutil.Parse(os.Args[1:])) {
	case ls.FullCommand():
		gs, err := newStorageClient(storage.DevstorageReadOnlyScope)
		if err != nil {
			panic(err)
		}
		printBuckets(gs, *lsProject)
	}
}

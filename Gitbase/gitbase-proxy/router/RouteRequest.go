package router

import (
	"fmt"
	"net/http"
	"proxy/config"
	"proxy/proxy"
)

func HostEntryToHostAddr(entry config.HostEntry) string {
	address := fmt.Sprintf("%s://%s:%s",
		entry.DestScheme, entry.DestHost, entry.DestPort,
	)
	return address
}

func getAddress(
	table []config.HostEntry,
	domainHost string,
) string {
	address := ""
	for _, entry := range table {
		if entry.DomainKey == domainHost {
			address = HostEntryToHostAddr(entry)
		}
	}
	return address
}

func processResult(
	address string,
	respWriter http.ResponseWriter,
	sourceRequest *http.Request,
) {
	if address == "" {
		fmt.Fprint(respWriter, "Domain is not valid.")
		respWriter.WriteHeader(500)
	} else {
		proxy.ProxyHandler(address, respWriter, sourceRequest)
	}
}

func RouteRequest(
	respWriter http.ResponseWriter,
	sourceRequest *http.Request,
) {
	domainHost := sourceRequest.Host
	table := config.Settings.ProxyTable
	address := getAddress(table, domainHost)
	fmt.Printf("Host: %s\nAddress: '%s'\n", domainHost, address)
	processResult(address, respWriter, sourceRequest)
}

import Feeds from 'pusher-feeds-client'
var feeds = new Feeds({
  instanceLocator: vv.boot_data.instanceLocator,
})

export default class PusherClient {
  constructor(feedName) {
    this.feed = feeds.feed(feedName);
  }

  subscribe(feedReceived) {
    this.feed.subscribe({
      previousItems: 20,
      onOpen: () => {
        console.log("Feeds: Connection established");
      },
      onItem: feedReceived,
      onError: error => {
        console.error("Feeds error:", error);
      },
    });

  }

}

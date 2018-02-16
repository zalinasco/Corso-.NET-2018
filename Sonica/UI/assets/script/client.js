var Solution = window.Solution || {};

Solution.Client = function () {

  /**
   * How many items to download. Albums and Playlists will always download all the elements.
   */
  var _itemsToDownload = localStorage.getItem('Client.ItemsToDownload') || 16;

  /**
   * Fetches the tracks. 
   * @returns {Promise} A promise.
   */
  var getTracks = function (albumName, artistName) {

    return Solution.Ajax.get(
      "tracks",
      {
        PageLength: _itemsToDownload,
        Album: albumName,
        Artist:artistName
      }
    ).then(
      function (data) {

        // TEST: image for web notifications
        var iconImage = 'http://funkyimg.com/i/21pX5.png';

        var trackList = [];

        for (var t in data) {
          trackList.push
          (
            {
              'icon': iconImage,
              'title': data[t].Title,
              'file': ApiBaseUrl + "tracks/" + data[t].Key + "/stream"
            }
          )
        }

        AP.init({playList: trackList});

      }
    );
  };

  /**
   * Fetches the albums. 
   * @returns {Promise} A promise.
   */
	var getAlbums = function () {

		return Solution.Ajax.get(
			"albums"
		).then(
			function (data) {



			}
			);
	};


  return {
    getTracks: getTracks
  };
}
  ();

Solution.Client.getTracks();

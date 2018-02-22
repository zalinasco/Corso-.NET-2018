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
							'file': ApiBaseUrl + "tracks/" + data[t].Key + "/stream",
							'imageurl': data[t].BackdropImageURL
            }
          )
        }

        AP.init({playList: trackList});

				if ($('.hamburger-menu').hasClass('slide')) {
					$('.hamburger-menu').click();
				}

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

				for (var i in data) {

					$("#albums")
						.append(
						"<button class='btn btn-default' onclick='Solution.Client.getTracks(\"" + data[i] + "\")'>" + data[i] + "</button>");

				}

			}
			);
	};


  /**
   * Fetches the artists. 
   * @returns {Promise} A promise.
   */
	var getArtists = function () {

		return Solution.Ajax.get(
			"artists"
		).then(
			function (data) {

				for (var i in data) {

					$("#artists")
						.append(
						"<button class='btn btn-default' onclick='Solution.Client.getTracks(null, \"" + data[i] + "\")'>" + data[i] + "</button>");

				}

			}
			);
	};


  return {
		getTracks: getTracks,
		getAlbums: getAlbums,
		getArtists: getArtists
  };
}
  ();

Solution.Client.getTracks();
Solution.Client.getAlbums();
Solution.Client.getArtists();

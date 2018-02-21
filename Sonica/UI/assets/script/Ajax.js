var Solution = window.Solution || {};

Solution.Ajax = function () {

	var _apiBaseUrl = typeof ApiBaseUrl !== "undefined" ? ApiBaseUrl : "http://10.10.10.37:5001/api/";

  /**
   * Performs an asynchronous GET to the specified url, passing the provided data.
   * @param {string} url The address of the resource.
   * @param {object} data The data to be passed.
   * @param {function(object)} dataAdapter A function to be used to adapt response data for various contexts.
   * @returns {Promise} A promise.
   */
  var get = function (url, data, dataAdapter, silent) {
    return ajax(url, data, "GET", dataAdapter, silent)
  }

  /**
   * Performs an asynchronous PUT to the specified url, passing the provided data.
   * @param {string} url The address of the resource.
   * @param {object} data The data to be passed.
   * @param {function(object)} dataAdapter A function to be used to adapt response data for various contexts.
   * @returns {Promise} A promise.
   */
  var put = function (url, data, dataAdapter, silent) {
    return ajax(url, data, "PUT", dataAdapter, silent)
  }

  /**
   * Performs an asynchronous POST to the specified url, passing the provided data.
   * @param {string} url The address of the resource.
   * @param {object} data The data to be passed.
   * @param {function(object)} dataAdapter A function to be used to adapt response data for various contexts.
   * @returns {Promise} A promise.
   */
  var post = function (url, data, dataAdapter, silent) {
    return ajax(url, data, "POST", dataAdapter, silent)
  }

  /**
   * Performs an asynchronous DELETE to the specified url, passing the provided data.
   * @param {string} url The address of the resource.
   * @param {object} data The data to be passed.
   * @param {function(object)} dataAdapter A function to be used to adapt response data for various contexts.
   * @returns {Promise} A promise.
   */
  var dele = function (url, data, dataAdapter, silent) {
    return ajax(url, data, "DELETE", dataAdapter, silent)
  }

  var _runningQueries = 0;

  /**
   * Performs an asynchronous operation to the specified url, passing the provided data.
   * @param {string} url The address of the resource.
   * @param {object} data The data to be passed.
   * @param {string} method The method, can be POST, GET, PUT, DELETE.
   * @param {function(object)} dataAdapter A function to be used to adapt response data for various contexts.
   * @returns {Promise} A promise.
   */
  var ajax = function (url, data, method, dataAdapter, silent) {
    var deferred = $.Deferred();

    _runningQueries++;

    $(".running-ajax").addClass("fa-spin");

    $.ajax({

      url: _apiBaseUrl + url,

      type: method,

      data: method === "GET" ? (data ? data : {}) : JSON.stringify(data ? data : {}),

      headers: {
        "accept": "application/json"
      },

      contentType: "application/json; charset=utf-8",

      cache: false,

      success: function (response) {
        if (typeof dataAdapter === 'function') {
          deferred.resolve(dataAdapter(response));
        } else {
          deferred.resolve(response);
        }
      },

      error: function (jqXHR, textStatus, errorThrown) {

        if (jqXHR.status !== 404 && jqXHR.responseText !== '') {
          var r = { 'Message': 'An unidentified error occurred.' };
          try {
            r = JSON.parse(jqXHR.responseText);
          } catch (e) { }
          var msg = r.Message + " " + (r.ExceptionMessage ? r.ExceptionMessage : "");
          console.log(msg);
        }

        deferred.reject(msg);

      },

      complete: function (jqXHR, textStatus) {
        _runningQueries--;
        if (_runningQueries === 0) {
          $(".running-ajax").removeClass("fa-spin");
        }
      }

    });

    return deferred.promise();
  }

  return {
    get: get,
    put: put,
    post: post,
    dele: dele,
  };
}
  ();


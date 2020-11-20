var app = angular.module("Spoticom", []);
angular.module("Spoticom").filter('trustThisUrl', ["$sce", function ($sce) {
    return function (val) {
        return $sce.trustAsResourceUrl(val);
    };
}]);
function onMyFrameLoad(iframe) {
    var yourUl = document.getElementById(iframe.id);
    yourUl.style.display = yourUl.style.display === 'none' ? '' : 'none';
    var loading = document.getElementById('Loading-' + iframe.id);
    loading.style.display = loading.style.display === 'none' ? '' : 'none';
}
app.controller('Ctrl1', function ($scope, $sce) {
    $(document).ready(function () {
        $(".mySlider").slider({});
    });
    var firstArtist = true;
    $scope.SuggestedArtists = [];
    $scope.SelectedArtists = [];
    $scope.SuggestedGenere = [];
    $scope.SelectedGenere = [];
    $scope.SelectedSongs = [];
    $("#SearchModal").on("hidden.bs.modal", function (e) {
        $("#main").css("z-index", "99999");
    });
    $("#Help").on("hidden.bs.modal", function (e) {
        $("#main").css("z-index", "99999");
    });
    var isSearchOpen = false;

    $scope.GoToAdvance = function () {
        if ($scope.SelectedSongs.length === 0) {
            alert("You must to select at least one song!");
        } else if ($scope.SelectedSongs.length > 2) {
            alert("Please select only 2 song");
        } else {
            $("#N5").fadeOut();
            setTimeout(function () {
                $("#Advance").fadeIn();
            }, 1000);
        }
    };
    $scope.SearchPending = false;
    $('.search-button').click(function () {
        if (!$(this).parent().hasClass('open')) {
            $(this).parent().toggleClass('open');
            $("#SearchInput").focus();
            isSearchOpen = true;
        } else {
            if ($scope.SearchText === "" || $scope.SearchText === null || $scope.SearchText === undefined) {
                alert("Please fill out the search input");
            } else {
                $("#SearchModal").modal();
                $("#main").css("z-index", "");
                $scope.SearchPending = true;
                var sTirdata = JSON.stringify({ Query: $scope.SearchText });
                $.ajax({
                    type: "Get",
                    url: "/WebMethods/SearchArtist?Query=" + $scope.SearchText,
                    data: sTirdata,
                    beforeSend: function () {

                    },
                    success: function (data) {
                        $scope.SearchPending = false;
                        $scope.ResultSearch = JSON.parse(data).artists.items;
                        $scope.$apply();
                    },
                    error: function (e) {
                        alert("Error!");
                        location.reload();
                    }
                });
            }

        }

    });
    $scope.start = function () {
        $("#N1").fadeOut();

        setTimeout(function () {
            $("#main").css("height", "");
            $("#main").css("width", "700px");
            $("#N2").fadeIn();

        }, 1000);
    };
    function onlyUnique(value, index, self) {
        return self.indexOf(value) === index;
    }
    $scope.StepGenere = function () {
        $("#N2").fadeOut();
        setTimeout(function () {
            $("#N3").fadeIn();
        }, 1000);
        for (var i = 0; i < $scope.SelectedArtists.length; i++) {
            for (var j = 0; j < $scope.SelectedArtists[i].genres.length; j++) {
                $scope.SuggestedGenere.push($scope.SelectedArtists[i].genres[j]);
            }
        }
        $scope.SuggestedGenere = $scope.SuggestedGenere.filter(onlyUnique);

    };
    $scope.MarketSelected = function (itm) {
        $scope.SelectedMarket = itm;
    };
    $scope.AddGenre = function (item) {
        if ($scope.SelectedGenere.length === 1) {
            alert("You cannot add any more genre!");
        } else {

            $("#N3").fadeOut();
            setTimeout(function () {
                $("#N4").fadeIn();
            }, 1000);
            $.ajax({
                type: "Get",
                url: "/WebMethods/GetMarkets",
                success: function (data) {
                    $scope.Markets = JSON.parse(data);
                    $scope.$apply();
                },
                error: function () {
                    alert("Error!");
                    location.reload();
                }
            });
        }

    };
    $scope.AddSuggestedArtist = function (item) {
        var exist = false;
        for (var i = 0; i < $scope.SelectedArtists.length; i++) {
            if ($scope.SelectedArtists[i].id === item.id) {
                exist = true;
            }
        }
        if (exist) {
            alert("You alredy add this artist to your list!");
        } else {
            if ($scope.SelectedArtists.length === 2) {
                alert("You cannot add any more artist!");
            } else {
                $scope.SelectedArtists.push(item);
                const index = $scope.SuggestedArtists.indexOf(item);
                if (index > -1) {
                    $scope.SuggestedArtists.splice(index, 1);
                }
            }
        }
    };
    $scope.AddArtistToList = function (item) {
        var exist = false;
        for (var i = 0; i < $scope.SelectedArtists.length; i++) {
            if ($scope.SelectedArtists[i].id === item.id) {
                exist = true;
            }
        }
        if (exist) {
            alert("You alredy add this artist to your list!");
        } else {
            if ($scope.SelectedArtists.length === 2) {
                alert("You cannot add any more artist!");
            } else {

                $scope.SelectedArtists.push(item);
                $('#SearchModal').modal('hide');
                $.ajax({
                    type: "Get",
                    url: "/WebMethods/SuggestArtist?ArtistId=" + item.id,
                    beforeSend: function () {

                    },
                    success: function (data) {
                        $scope.SuggestedArtists = JSON.parse(data).artists;
                        $scope.$apply();
                    },
                    error: function () {
                        alert("Error!");
                        location.reload();
                    }
                });
                //SuggestArtist

            }

        }
    };
    $scope.BackToSetting = function () {
        $("#N7").fadeOut();
        setTimeout(function () {
            $("#Advance").fadeIn();
        }, 1000);
        $scope.Recoms = [];
    };
    $scope.StepDone = function () {
        var Dancablity = $("#Dancablity").val();
        var Acousticness = $("#Acousticness").val();
        var Energy = $("#Energy").val();
        var Instrumentalness = $("#Instrumentalness").val();
        var artistsIds = "";
        var Tracks = "";
        var genre = "";
        for (var i = 0; i < $scope.SelectedArtists.length; i++) {
            artistsIds += $scope.SelectedArtists[i].id + ",";
        }
        artistsIds = artistsIds.substring(0, artistsIds.length - 1);
        for (var j = 0; j < $scope.SelectedSongs.length; j++) {
            Tracks += $scope.SelectedSongs[j].id + ",";
        }
        if (artistsIds.substr(artistsIds.length - 1) === ",") {
            artistsIds = artistsIds.substring(0, artistsIds.length - 1);
        }
        if (Tracks.substr(Tracks.length - 1) === ",") {
            Tracks = Tracks.substring(0, Tracks.length - 1);
        }
        genre = $scope.SelectedGenere[0];
        $.ajax({
            type: "Get",
            url: "/WebMethods/GetRecommandationWithAudioFeatures?ArtistIds=" + artistsIds + "&Genre=" + genre + "&TracksIds=" + Tracks + "&Market=" + $scope.SelectedMarket + "&Dancablity=" + Dancablity + "&Acousticness=" + Acousticness + "&Energy=" + Energy + "&Instrumentalness=" + Instrumentalness,
            beforeSend: function () {
                $("#Loading-Recom").show();
            },
            success: function (data) {
                $("#Loading-Recom").hide();
                if (JSON.parse(data).length === 0) {
                    alert("There is no song available With this audio feature settings. Read help or skip this step");
                } else {
                    $("#Advance").fadeOut();
                    setTimeout(function () {
                        $("#N7").fadeIn();
                    }, 1000);
                    $scope.Recoms = JSON.parse(data);
                    $scope.TasteList = [];
                    $scope.$apply();
                }

            },
            error: function () {
                alert("Error!");
                location.reload();

            }
        });
    };
    $scope.GetRecommandationI = function () {
        //GetRecommandation
        $.ajax({
            type: "Get",
            url: "/WebMethods/SuggestArtist?ArtistId=" + item.id,
            beforeSend: function () {

            },
            success: function (data) {
                $scope.SuggestedArtists = JSON.parse(data).artists;
                //$scope.SuggestedArtists = $scope.SuggestedArtists.filter(onlyUnique);
                $scope.$apply();
            },
            error: function () {
                alert("Error!");
                location.reload();
            }
        });
    };
    $scope.SongChanged = function (itm) {
        if (itm.Check) {
            itm.Check = false;
            const index = $scope.SelectedSongs.indexOf(itm);
            if (index > -1) {
                $scope.SelectedSongs.splice(index, 1);
            }
        } else {
            if ($scope.SelectedSongs.length >= 2) {
                alert("Remmember, You can select only 2 song!");
            }
            itm.Check = true;
            $scope.SelectedSongs.push(itm);

        }


    };
    $scope.Help = function () {
        $("#Help").modal();
        $("#main").css("z-index", "");
    };
    $scope.GetRecommandationI = function () {
        var artistsIds = "";
        var Tracks = "";
        var genre = "";
        for (var i = 0; i < $scope.SelectedArtists.length; i++) {
            artistsIds += $scope.SelectedArtists[i].id + ",";
        }
        artistsIds = artistsIds.substring(0, artistsIds.length - 1);
        for (var j = 0; j < $scope.SelectedSongs.length; j++) {
            Tracks += $scope.SelectedSongs[j].id + ",";
        }
        if (artistsIds.substr(artistsIds.length - 1) === ",") {
            artistsIds = artistsIds.substring(0, artistsIds.length - 1);
        }
        if (Tracks.substr(Tracks.length - 1) === ",") {
            Tracks = Tracks.substring(0, Tracks.length - 1);
        }
        genre = $scope.SelectedGenere[0];
        $.ajax({
            type: "Get",
            url: "/WebMethods/GetRecommandation?ArtistIds=" + artistsIds + "&Genre=" + genre + "&TracksIds=" + Tracks + "&Market=" + $scope.SelectedMarket,
            beforeSend: function () {

            },
            success: function (data) {
                $scope.Recoms = JSON.parse(data);
                $scope.TasteList = [];
                $scope.$apply();
            },
            error: function (e) {
                alert("Error!");
                location.reload();
            }
        });
        $("#Advance").fadeOut();
        setTimeout(function () {
            $("#N7").fadeIn();
        }, 1000);
    };
    $scope.removeArtist = function (itm) {
        const index = $scope.SelectedArtists.indexOf(itm);
        if (index > -1) {
            $scope.SelectedArtists.splice(index, 1);
        }
    };
    $scope.StepTrack = function () {
        var artistsIds = "";
        for (var i = 0; i < $scope.SelectedArtists.length; i++) {
            artistsIds += $scope.SelectedArtists[i].id + ",";
        }
        $("#Loading-songsc").show();
        artistsIds = artistsIds.substring(0, artistsIds.length - 1);
        $.ajax({
            type: "Get",
            url: "/WebMethods/GetTastesTestSongs?ArtistIds=" + artistsIds + "&Market=" + $scope.SelectedMarket,
            beforeSend: function () {

            },
            success: function (data) {
                $("#Loading-songsc").hide();
                if (JSON.parse(data).length === 0) {
                    alert("There is no song availabe for this country");
                } else {
                    $("#N4").fadeOut();
                    setTimeout(function () {
                        $("#N5").fadeIn();
                    }, 1000);
                    $scope.TasteList = JSON.parse(data);
                    $scope.$apply();
                }
            },
            error: function (e) {
                alert("Error!");
                location.reload();

            }
        });

    };


});

var songsList = [
    {
        "title": "New Rules",
        "artist": "Dua Lipa",
        "link": "http://mysound.ge/uploads/tracks/729760672_1526780201_1460816669.mp3",
        "thumb": "https://a10.gaanacdn.com/images/albums/47/1908547/crop_175x175_1908547.jpg"
    },
    {
        "title": "Señorita",
        "artist": "Shawn Mendes, Camila Cabello",
        "link": "http://mysound.ge/uploads/tracks/2135911823_315158852_1994375449.mp3",
        "thumb": "https://a10.gaanacdn.com/images/albums/72/2657072/crop_175x175_2657072.jpg"
    },
    {
        "title": "Wake Me Up",
        "artist": "Avicii",
        "link": "http://mysound.ge/uploads/tracks/1932929623_1281390459_1755572096.mp3",
        "thumb": "https://i1.sndcdn.com/artworks-000143888529-4mwefu-t500x500.jpg"
    }

];

var currIndex = 0;
var isPlaying = false;
var currSong = new Audio();

var songThumb = document.querySelector(".song-thumb");
var songTitle = document.querySelector(".song-info-title");
var songArtist = document.querySelector(".song-info-artist");
var songAlbum = document.querySelector(".song-info-album");

var stateButton = document.querySelector(".player-state-btn");
var songProgressBar = document.querySelector(".song-progress-value");
var volumeSlider = document.querySelector("#volume-slider");
var volumeTrail = document.querySelector(".volume-trail");

function changeSong() {
    let currentStatus = isPlaying;
    if (currentStatus) toggleState();
    songTitle.innerHTML = songsList[currIndex]["title"];
    songArtist.innerHTML = songsList[currIndex]["artist"];
    songThumb.style.backgroundImage = `url(${songsList[currIndex]["thumb"]})`;
    currSong.src = songsList[currIndex]["link"];
    if (currentStatus) toggleState();
}
function nextSong() {
    currIndex++;
    if (currIndex >= songsList.length) currIndex = 0;
    changeSong();
}
function prevSong() {
    currIndex--;
    if (currIndex < 0) currIndex = songsList.length - 1;
    changeSong();
}
function toggleState() {
    if (isPlaying) {
        currSong.pause();
        stateButton.classList = "fas fa-play-circle player-state-btn";
    }
    else {
        currSong.play();
        stateButton.classList = "fas fa-pause-circle player-state-btn";
    }
    isPlaying = !isPlaying;
}
function adjustVolume(currVol) {
    currSong.volume = currVol;
    console.log(currVol, currVol !== "0", currVol !== 0);
    if (currVol !== "0" && currVol !== 0)
        volumeTrail.style.width = `${currVol * 100 - 2}%`;
    else
        volumeTrail.style.width = "0%";
    volumeSlider.value = currVol;
}
// nextSong();
currSong.addEventListener('timeupdate', () => {
    let currPosition = currSong.currentTime / currSong.duration * 600;
    if (!isNaN(currPosition))
        songProgressBar.setAttribute("stroke-dasharray", `${currPosition} ${600 - currPosition}`);
    else
        songProgressBar.setAttribute("stroke-dasharray", "0 600");
});

$(document).ready(function () {
    changeSong();
});
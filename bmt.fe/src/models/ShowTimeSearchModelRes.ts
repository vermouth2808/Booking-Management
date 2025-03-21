export interface ShowTimeSearchModelRes {
totalRecords: number;
showTimes: ShowTime[];
}

export interface ShowTime{
    showtimeId: number;
    movieId: number;
    roomId: number;
    startTime: Date;   
    endTime: Date;
    price?: number;
    title: string;
    director: string;
    performer: string;
    language: string;
    genre: string;
    duration: number;
    releaseDate: Date;
    posterUrl: string;
    trailerUrl: string;
    ageRating: string;
    roomName: string;
}
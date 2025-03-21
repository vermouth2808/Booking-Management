import React, { useEffect, useState } from "react";
import { Collapse, Tag, Button } from "antd";
import { useSelector } from "react-redux";
import ShowTimeService from "../../services/ShowTimeService";
import "./ShowtimeSchedule.css";
import Seat from "../Seat/Seat";
import { ShowTime } from "../../models/ShowTimeSearchModelRes";
import { SearchShowTimeModelReq } from "../../models/SearchShowTimeModelReq";

const { Panel } = Collapse;

interface GroupedShowtimes {
  [roomId: number]: {
    roomName: string;
    showtimes: string[];
  };
}

interface ShowtimeScheduleProps {
  movieId: number;
}

const ShowtimeSchedule: React.FC<ShowtimeScheduleProps> = ({ movieId }) => {
  const darkMode = useSelector((state: any) => state.theme.darkMode);
  const [showtimes, setShowtimes] = useState<ShowTime[]>([]);
  const [listRoom, setListRoom] = useState<ShowTime[]>([]);
  const [groupedShowtimes, setGroupedShowtimes] = useState<GroupedShowtimes>({});
  const [selectedShowtimeDate, setSelectedShowtimeDate] = useState<Date>();
  const [selectedRoomId, setSelectedRoomId] = useState<number | null>(null);

  useEffect(() => {
    if (showtimes.length > 0) {
      const sortedShowtimes = [...showtimes].sort((a, b) => new Date(a.startTime).getTime() - new Date(b.startTime).getTime());
      setSelectedShowtimeDate(sortedShowtimes[0].startTime);
    }
  }, [showtimes]);

  useEffect(() => {
    const fetchShowTime = async () => {
      try {
        const now = new Date();
        const params : SearchShowTimeModelReq = {
          keySearch: "",
          pageSize: 10,
          pageIndex: 1,
          fromDate: new Date(),
          toDate: new Date(now.getFullYear(), now.getMonth() + 1, 0),
          movieId: movieId,
          roomId: undefined,
        };
        const showtime: ShowTime[] | undefined = await ShowTimeService.SearchShowTime(params);
        setShowtimes(Array.isArray(showtime) ? showtime : []);
      } catch (error) {
        console.log("Load showtimes failed", error);
      }
    };
    fetchShowTime();
  }, [movieId]);

  useEffect(() => {
    const fetchShowTime = async () => {
      try {
        if (!selectedShowtimeDate) return;
        const params : SearchShowTimeModelReq = {
          keySearch: "",
          pageSize: 10,
          pageIndex: 1,
          fromDate: selectedShowtimeDate,
          toDate: selectedShowtimeDate,
          movieId: movieId, 
          roomId: undefined,
        };
        const showtime: ShowTime[]| undefined = await ShowTimeService.SearchShowTime(params);
        setListRoom(Array.isArray(showtime) ? showtime : []);
      } catch (error) {
        console.log("Load showtimes failed", error);
      }
    };
    fetchShowTime();
  }, [selectedShowtimeDate]);


  useEffect(() => {
    const grouped = listRoom.reduce<GroupedShowtimes>((acc, showtime) => {
      if (!acc[showtime.roomId]) {
        acc[showtime.roomId] = {
          roomName: showtime.roomName,
          showtimes: [],
        };
      }
      const date = new Date(showtime.startTime).toISOString();
      const timePart = date.includes("T")
        ? date.split("T")[1]?.slice(0, 5)
        : "00:00";
      acc[showtime.roomId].showtimes.push(timePart);
      return acc;
    }, {});
    setGroupedShowtimes(grouped);
  }, [listRoom]);

  return (
    <div className={`detail-container ${darkMode ? "dark" : "light"}`}>
      <h2 className="title">
        <span className="movie-title">LỊCH CHIẾU</span>
      </h2>

      {showtimes.length > 0 ? (
        <div className="date-picker">
          {Array.from(new Set(showtimes.map((s) => new Date(s.startTime)))).slice(0,7).map(
            (date) => (
              <Button
                key={date.toISOString()}
                size="large"
                className={`date-btn ${
                  selectedShowtimeDate === date ? "active" : ""
                }`}
                onClick={() => setSelectedShowtimeDate(date)}
              >
                <span>
                  {date.toLocaleDateString("vi-VN", {
                    day: "2-digit",
                    month: "2-digit",
                  })}
                  <br />
                  {date.toLocaleDateString("vi-VN", { weekday: "long" })}
                </span>
              </Button>
            )
          )}
        </div>
      ) : (
        <p className="no-movie">Chưa có lịch chiếu.</p>
      )}

      <h3 className="subtitle">
        <span className="movie-title">DANH SÁCH PHÒNG</span>
      </h3>

      {Object.keys(groupedShowtimes).length > 0 ? (
        <Collapse className="cinema-list" accordion>
          {Object.entries(groupedShowtimes).map(
            ([roomId, { roomName, showtimes }]) => (
              <Panel
                header={<span className="cinema-title">{roomName}</span>}
                key={roomId}
                className="cinema-panel"
              >
                <div className="showtime-buttons">
                  {showtimes.map((time: string, index: number) => (
                    <Tag
                      key={index}
                      onClick={() => setSelectedRoomId(Number(roomId))}
                      className="showtime"
                    >
                      {time}
                    </Tag>
                  ))}
                </div>
              </Panel>
            )
          )}
        </Collapse>
      ) : (
        <p>Vui lòng chọn ngày chiếu phim.</p>
      )}

      <Seat RoomId={selectedRoomId} />
    </div>
  );
};

export default ShowtimeSchedule;

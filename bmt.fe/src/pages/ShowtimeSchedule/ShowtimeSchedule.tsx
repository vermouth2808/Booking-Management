import React, { useEffect, useState, useCallback } from "react";
import { Collapse, Tag, Button } from "antd";
import { useSelector } from "react-redux";
import ShowTimeService from "../../services/ShowTimeService";
import "./ShowtimeSchedule.css";
import Seat from "../Seat/Seat";
import { RootState } from "../../store";
import { ShowTime } from "../../models/ShowTimeSearchModelRes";
import { SearchShowTimeModelReq } from "../../models/SearchShowTimeModelReq";

const { Panel } = Collapse;

interface ShowtimeScheduleProps {
  movieId: number;
  onSeatSelect: (seats: string[]) => void;
}

interface GroupedShowtimes {
  [roomId: number]: {
    roomName: string;
    showtimes: string[];
  };
}

const ShowtimeSchedule: React.FC<ShowtimeScheduleProps> = ({ movieId, onSeatSelect }) => {
  const darkMode = useSelector((state: RootState) => state.theme.darkMode);
  const [showtimes, setShowtimes] = useState<ShowTime[]>([]);
  const [listRoom, setListRoom] = useState<ShowTime[]>([]);
  const [groupedShowtimes, setGroupedShowtimes] = useState<GroupedShowtimes>({});
  const [selectedShowtimeDate, setSelectedShowtimeDate] = useState<Date>(new Date());
  const [selectedRoomId, setSelectedRoomId] = useState<number | null>(null);
  const [selectedSeats, setSelectedSeats] = useState<string[]>([]);

  const handleSeatSelection = (seats: string[]) => {
    setSelectedSeats(seats);
    onSeatSelect(seats); // Truyền lên DetailMovie
  };

  const fetchShowTime = useCallback(async (fromDate: Date, toDate: Date) => {
    try {
      const params: SearchShowTimeModelReq = {
        keySearch: "",
        pageSize: 10,
        pageIndex: 1,
        fromDate: selectedShowtimeDate,
        toDate: selectedShowtimeDate,
        movieId,
        roomId: undefined,
      };
      const showtime: ShowTime[] = (await ShowTimeService.SearchShowTime(params)) ?? [];
      return Array.isArray(showtime) ? showtime : [];
    } catch (error) {
      console.error("⚠️ Load showtimes failed", error);
      return [];
    }
  }, [movieId, selectedShowtimeDate]);

  useEffect(() => {
    const loadInitialShowTimes = async () => {
      const now = new Date();
      const fetchedShowtimes = await fetchShowTime(now, new Date(now.getFullYear(), now.getMonth() + 1, 0));
      setShowtimes(Array.isArray(fetchedShowtimes) ? fetchedShowtimes : []);
    };
    loadInitialShowTimes();
  }, [fetchShowTime]);

  useEffect(() => {
    const loadRoomsForDate = async () => {
      if (!selectedShowtimeDate) return;
      const fetchedRooms = await fetchShowTime(selectedShowtimeDate, selectedShowtimeDate);
      setListRoom(Array.isArray(fetchedRooms) ? fetchedRooms : []);
    };
    loadRoomsForDate();
  }, [selectedShowtimeDate, fetchShowTime]);

  useEffect(() => {
    if (!Array.isArray(listRoom)) {
      console.error("", listRoom);
      return;
    }

    const grouped = listRoom.reduce<GroupedShowtimes>((acc, showtime) => {
      if (!showtime.roomId || !showtime.roomName) return acc;
      if (!acc[showtime.roomId]) {
        acc[showtime.roomId] = {
          roomName: showtime.roomName,
          showtimes: [],
        };
      }
      acc[showtime.roomId].showtimes.push(
        new Date(showtime.startTime ?? "").toISOString().split("T")[1].slice(0, 5)
      );
      return acc;
    }, {});

    setGroupedShowtimes(grouped);
  }, [listRoom]);

  return (
    <div className={`detail-container ${darkMode ? "dark" : "light"}`}>
      <h2 className="title">LỊCH CHIẾU</h2>

      {showtimes.length > 0 ? (
        <div className="date-picker">
          {showtimes.map((showtimeItem) => {
            const dateObj = new Date(showtimeItem.startTime);
            const formattedDate = dateObj.toLocaleDateString("vi-VN");
            const dayOfWeek = dateObj.toLocaleDateString("vi-VN", {
              weekday: "long",
            });
            return (
              <Button
                size="large"
                key={showtimeItem.showtimeId}
                className={`date-btn ${
                  selectedShowtimeDate.getTime() === dateObj.getTime()
                    ? "active"
                    : ""
                }`}
                onClick={() => setSelectedShowtimeDate(dateObj)}
              >
                <span>
                  {formattedDate}
                  <br />
                  {dayOfWeek}
                </span>
              </Button>
            );
          })}
        </div>
      ) : (
        <p className="no-movie">Chưa có lịch chiếu.</p>
      )}

      <h3 className="subtitle">DANH SÁCH PHÒNG</h3>

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
                  {showtimes.map((time : string, index : number) => (
                    <Tag
                      onClick={() => setSelectedRoomId(Number(roomId))}
                      className="showtime"
                      key={index}
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

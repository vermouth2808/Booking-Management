import React, { useEffect, useState } from "react";
import { Collapse, Tag, Button } from "antd";
import { useSelector } from "react-redux";
import ShowTimeService from "../../services/ShowTimeService";
import "./ShowtimeSchedule.css";
import Seat from "../Seat/Seat"

const { Panel } = Collapse;

const ShowtimeSchedule = ({ movieId }) => {
  const darkMode = useSelector((state) => state.theme.darkMode);
  const [showtimes, setShowtimes] = useState([]);
  const [listRoom, setListRoom] = useState([]);
  const [groupedShowtimes, setGroupedShowtimes] = useState({}); // Thêm state cho groupedShowtimes
  const [selectedShowtimeDate, setSelectedShowtimeDate] = useState(null);
  const [selectedRoomId, setSelectedRoomId] = useState(null);
  

  useEffect(() => {
    if (showtimes.length > 0) {
      const sortedShowtimes = [...showtimes].sort((a, b) => new Date(a.startTime) - new Date(b.startTime));
      setSelectedShowtimeDate(sortedShowtimes[0].startTime); // Chọn ngày nhỏ nhất
    }
  }, [showtimes]);



  useEffect(() => {
    const fetchShowTime = async () => {
      try {
        const now = new Date();
        const params = {
          keySearch: "",
          pageSize: 10,
          pageIndex: 1,
          fromDate: new Date(),
          toDate: new Date(now.getFullYear(), now.getMonth() + 1, 0).toISOString(),
          movieId: parseInt(movieId, 10),
          roomId: null,
        };
        const showtime = await ShowTimeService.SearchShowTime(params);
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
        const now = new Date();
        const params = {
          keySearch: "",
          pageSize: 10,
          pageIndex: 1,
          fromDate: selectedShowtimeDate,
          toDate: selectedShowtimeDate,
          movieId: parseInt(movieId, 10),
          roomId: null,
        };
        const showtime = await ShowTimeService.SearchShowTime(params);
        setListRoom(Array.isArray(showtime) ? showtime : []);
      } catch (error) {
        console.log("Load showtimes failed", error);
      }
    };
    fetchShowTime();
  }, [selectedShowtimeDate]);

  useEffect(() => {
    // Nhóm suất chiếu theo roomId
    const grouped = listRoom.reduce((acc, showtime) => {
      if (!acc[showtime.roomId]) {
        acc[showtime.roomId] = {
          roomName: showtime.roomName,
          showtimes: [],
        };
      }
      acc[showtime.roomId].showtimes.push(showtime.startTime.split('T')[1].slice(0, 5));
      return acc;
    }, {});

    setGroupedShowtimes(grouped); 
  }, [listRoom]); 

  return (
    <div className={`detail-container ${darkMode ? "dark" : "light"}`}>
      <h2 className="title">
        <span className="movie-title">LỊCH CHIẾU</span>
      </h2>

      {/* Hiển thị danh sách ngày chiếu (không trùng lặp) */}
      {showtimes.length > 0 ? (
        <div className="date-picker">
          {(() => {
            const uniqueDates = new Map();

            return showtimes.map((showtimeItem) => {
              const startTime = showtimeItem.startTime;
              const dateObj = new Date(startTime);
              const formattedDate = dateObj.toLocaleDateString("vi-VN");
              const dayOfWeek = dateObj.toLocaleDateString("vi-VN", { weekday: "long" });

              // Kiểm tra nếu ngày đã có trong Map thì bỏ qua
              if (uniqueDates.has(formattedDate)) {
                return null;
              }

              uniqueDates.set(formattedDate, showtimeItem.showtimeId);

              return (
                <Button
                  size="large"
                  key={showtimeItem.showtimeId}
                  className={`date-btn ${selectedShowtimeDate === showtimeItem.startTime ? "active" : ""}`}
                  onClick={() =>
                    setSelectedShowtimeDate(showtimeItem.startTime)
                  }
                >
                  <span>
                    {formattedDate}
                    <br />
                    {dayOfWeek}
                  </span>
                </Button>
              );
            });
          })()}
        </div>
      ) : (
        <p className="no-movie">Chưa có lịch chiếu.</p>
      )}

      <h3 className="subtitle">
        <span className="movie-title">DANH SÁCH PHÒNG</span>
      </h3>

      {/* Danh sách phòng chiếu */}
      {Object.entries(groupedShowtimes).length > 0 ? (
        <Collapse className="cinema-list" accordion>
          {Object.entries(groupedShowtimes).map(([roomId, { roomName, showtimes }]) => (
            <Panel header={
              <span className="cinema-title">
                {roomName}
              </span>}
              key={roomId} className="cinema-panel">
              <div className="showtime-buttons">
                {showtimes.map((time, index) => (
                  <Tag
                    onClick={() => setSelectedRoomId(roomId)}
                    className="showtime" key={index}>
                    {time}
                  </Tag>
                ))}
              </div>
            </Panel>
          ))}
        </Collapse>
      ) : (
        <p>Vui lòng chọn ngày chiếu phim.</p>
      )}


      {/*sơ đồ ghế trong phòng*/}
    <Seat RoomId={selectedRoomId}/>
    </div>
  );
};

export default ShowtimeSchedule;

import React, { useState } from "react";
import { Card, Button, Select, Collapse, Tag } from "antd";
import { EnvironmentOutlined } from "@ant-design/icons";
import { useSelector } from "react-redux";
import "./ShowtimeSchedule.css";

const { Option } = Select;
const { Panel } = Collapse;

const showtimesData = [
  {
    id: 1,
    cinema: "Cinestar Quốc Thanh (TP.HCM)",
    address: "271 Nguyễn Trãi, Phường Nguyễn Cư Trinh, Quận 1, TP.HCM",
    type: "Standard",
    times: ["11:00", "16:45", "18:30"],
  },
  {
    id: 2,
    cinema: "Cinestar Hai Bà Trưng (TP.HCM)",
    address: "135 Hai Bà Trưng, Phường Bến Nghé, Quận 1, TP.HCM",
    type: "Deluxe",
    times: ["11:45", "13:25"],
  },
];

const ShowtimeSchedule = () => {
  const darkMode = useSelector((state) => state.theme.darkMode);
  const [selectedDate, setSelectedDate] = useState("11/03");
  const [selectedCity, setSelectedCity] = useState("Hồ Chí Minh");

  return (
    <div className={`detail-container ${darkMode ? "dark" : "light"}`}>
      <h2 className="title"><span className="movie-title">LỊCH CHIẾU</span></h2>
      <div className="date-picker">
        {["11/03", "12/03", "13/03"].map((date) => (
          <Button
            size="large"
            key={date}
            className={`date-btn ${selectedDate === date ? "active" : ""}`}
            onClick={() => setSelectedDate(date)}
          >
           <span>
           {date}
            <br />
            {date === "11/03" ? "Thứ Ba" : date === "12/03" ? "Thứ Tư" : "Thứ Năm"}
            </span> 
          </Button>
        ))}
      </div>


      <h3 className="subtitle"><span className="movie-title">DANH SÁCH PHÒNG</span></h3>
      <Collapse className="cinema-list" accordion>
        {showtimesData.map((cinema) => (
          <Panel
            header={<span className="cinema-title">{cinema.cinema}</span>}
            key={cinema.id}
            className="cinema-panel"
          >
            <p className="cinema-address">{cinema.address}</p>
            <p className="cinema-type">{cinema.type}</p>
            <div className="showtime-buttons">
              {cinema.times.map((time, index) => (
                <Tag key={index} className={`showtime ${index === 0 ? "disabled" : ""}`}>
                  {time}
                </Tag>
              ))}
            </div>
          </Panel>
        ))}
      </Collapse>
    </div>
  );
};

export default ShowtimeSchedule;

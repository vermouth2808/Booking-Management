import React, { useEffect, useState } from "react";
import { Typography, Tag, Button, Modal, Tooltip } from "antd";
import { useParams } from "react-router-dom";
import { ClockCircleOutlined, UserOutlined, GlobalOutlined, VideoCameraOutlined, StarOutlined, SmileOutlined, PlayCircleOutlined } from "@ant-design/icons";
import "../detailMovie/DetailMovie.css";
import { useSelector } from "react-redux";
import MovieService from "../../services/MovieService";
import ShowtimeSchedule from "../ShowtimeSchedule/ShowtimeSchedule"

const { Title, Text, Paragraph } = Typography;

const DetailMovie = () => {
  const darkMode = useSelector((state) => state.theme.darkMode);
  const { id } = useParams();
  const [movie, setMovie] = useState(null);
  const [isExpanded, setIsExpanded] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);

  useEffect(() => {
    const fetchMovieDetail = async () => {
      try {
        const movieDetail = await MovieService.movieDetail(id);
        setMovie(movieDetail);
      } catch (error) {
        console.error("Error fetching movies:", error);
      }
    };

    fetchMovieDetail();
  }, [id]);
  return (
    <div className={`detail-container ${darkMode ? "dark" : "light"}`}>
      {movie ? (
        <div className="detail-movie">
          {/* Poster Phim */}
          <div className="movie-poster">
            <img src={movie.posterUrl || "https://via.placeholder.com/400x600"} alt={movie.title} />
          </div>

          {/* Thông Tin Phim */}
          <div className="movie-info">
            <Title level={2}><span className="movie-title">{movie.title}</span></Title>

            <div className="movie-age">
              {/* Thể loại */}
              <Tag icon={<StarOutlined />} color="gold">
                <span className="movie-genre">Thể loại: {movie.genre}</span>
              </Tag>
            </div>
            <div className="movie-age">
              {/* Thời lượng */}
              <Tag icon={<ClockCircleOutlined />} color="blue">
                Thời lượng: {movie.duration || "84'"}
              </Tag>
            </div>
            <div className="movie-age">
              {/* Đạo diễn */}
              <Tag icon={<VideoCameraOutlined />} color="purple">
                Đạo diễn: {movie.director}
              </Tag>
            </div>
            <div className="movie-age">
              {/* Diễn viên có tooltip */}
              <Tooltip title={movie.performer} mouseEnterDelay={0.5}>
                <Tag icon={<UserOutlined />} color="purple">
                  <span className="movie-category">
                    Diễn viên: {movie.performer?.length > 50 ? movie.performer.substring(0, 50) + "..." : movie.performer}
                  </span>
                </Tag>
              </Tooltip>
            </div>
            <div className="movie-age">
              {/* Ngôn ngữ */}
              <Tag icon={<GlobalOutlined />} color="red">
                Ngôn ngữ: {movie.language}
              </Tag>
            </div>
            <div className="movie-age">
              {/* Độ tuổi */}
              <Tag icon={<SmileOutlined />} color="yellow">
                P: {movie.ageRating}
              </Tag>
            </div>

            {/* Nút Xem Trailer */}
            {movie.trailerUrl && (
              <Button
                type="primary"
                shape="round"
                icon={<PlayCircleOutlined />}
                size="large"
                onClick={() => setIsModalOpen(true)}
              >
                Xem Trailer
              </Button>
            )}
            <div className="movie-age">
              {/* Mô tả */}
              <Title level={4}><span className="movie-title">MÔ TẢ</span></Title>
              <Text><span className="movie-releaseDate">Khởi chiếu: {movie.releaseDate}</span></Text>
            </div>
            {/* Nội dung phim */}
            <Title level={4}><span className="movie-title">NỘI DUNG PHIM</span></Title>
            <Paragraph
              className={`movie-description ${isExpanded ? "expanded" : ""}`}
              ellipsis={isExpanded ? false : { rows: 2, expandable: false }}
            >
              {movie.description || "Mô tả phim chưa có."}
            </Paragraph>
            <Button type="link" onClick={() => setIsExpanded(!isExpanded)}>
              {isExpanded ? "Thu gọn" : "Đọc thêm"}
            </Button>

          </div>

          {/* Modal Trailer */}
          {movie.trailerUrl && (
            <Modal
              title={`Trailer phim: ${movie.title}`}
              open={isModalOpen}
              onCancel={() => setIsModalOpen(false)}
              footer={null}
              width={800}
              centered
            >
              <iframe
                width="100%"
                height="400"
                src={movie.trailerUrl}
                title="YouTube video player"
                frameBorder="0"
                allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
                referrerPolicy="strict-origin-when-cross-origin"
                allowFullScreen
              ></iframe>
            </Modal>
          )}
        </div>
      ) : (
        <p className="no-movie">Không có phim nào để hiển thị.</p>
      )}
      
      <ShowtimeSchedule  movieId={id}/>

    </div>

  );
};

export default DetailMovie;

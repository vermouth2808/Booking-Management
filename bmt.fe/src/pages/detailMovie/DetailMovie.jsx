import React, { useEffect, useState } from "react";
import { Typography, Tag, Button, Modal } from "antd";
import { useParams } from "react-router-dom";
import { ClockCircleOutlined, PlayCircleOutlined } from "@ant-design/icons";
import "../detailMovie/DetailMovie.css";
import { useSelector } from "react-redux";
import MovieService from "../../services/MovieService";

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
        <>
          <div className="movie-poster">
            <img src={movie.posterUrl || "https://via.placeholder.com/400x600"} alt={movie.title} />
          </div>

          <div className="movie-info">
            <Title level={2}><span className="movie-title">{movie.title} (P) - ID: {id}</span> </Title>
            <div className="movie-tags">
              <Tag color="gold"><span className="movie-genre">{movie.genre}</span></Tag>
              <Tag icon={<ClockCircleOutlined />} color="blue">
                {movie.duration || "84'"}
              </Tag>
              <Tag color="purple"><span className="movie-category">{movie.category}</span> </Tag>
              <Tag color="red">Phụ Đề</Tag>
            </div>

            <div className="movie-age">
              <Tag color="yellow">🎭 P: {movie.ageRating || "Phim dành cho khán giả mọi lứa tuổi"}</Tag>
            </div>

            <div>
              <Title level={4}><span className="movie-title">MÔ TẢ</span></Title>
              <Text><span className="movie-releaseDate">Khởi chiếu: {movie.releaseDate}</span></Text>

              <Title level={4}><span className="movie-title">NỘI DUNG PHIM</span></Title>
              <Paragraph
                className={`movie-description ${isExpanded ? "expanded" : ""}`}
                ellipsis={!isExpanded ? { rows: 2, expandable: false } : false}
              >
                {movie.description || "Mô tả phim chưa có."}
              </Paragraph>
              <Button
                type="link"
                onClick={() => setIsExpanded(!isExpanded)}
              >
                {isExpanded ? "Thu gọn" : "Đọc thêm"}
              </Button>
            </div>

            {movie.trailerUrl && (
              <>
                <Button
                  type="primary"
                  shape="round"
                  icon={<PlayCircleOutlined />}
                  size="large"
                  onClick={() => setIsModalOpen(true)}
                >
                  Xem Trailer
                </Button>

                <Modal
                  title={`Trailer phim : ${movie.title}`}
                  open={isModalOpen}
                  onCancel={() => setIsModalOpen(false)}
                  footer={null}
                  width={800}
                  centered
                >
                  <iframe width= "100%" height="400" src="https://www.youtube.com/embed/Yz96EBNwMGw?si=T7pSIpVvyNMmGdkD"
                    title="YouTube video player" frameborder="0"
                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
                    referrerpolicy="strict-origin-when-cross-origin" allowfullscreen></iframe>
                </Modal>
              </>
            )}
          </div>
        </>
      ) : (
        <p className="no-movie">Không có phim nào để hiển thị.</p>
      )}
    </div>
  );
};

export default DetailMovie;

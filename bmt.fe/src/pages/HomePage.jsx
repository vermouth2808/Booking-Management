import { Button, Carousel, Layout, Rate, Typography } from "antd";
import { motion } from "framer-motion";
import React, { useEffect, useState } from "react";
import MovieService from "../services/MovieService";
import { useSelector } from "react-redux";
import { Link } from "react-router-dom";

const { Content } = Layout;
const { Title } = Typography;

const HomePage = () => {
  const darkMode = useSelector((state) => state.theme.darkMode);
  const [movies, setMovies] = useState([]);

  useEffect(() => {
    const controller = new AbortController();
    const fetchMovies = async () => {
      try {
        const moviesData = await MovieService.searchMovies(controller.signal);
        setMovies(Array.isArray(moviesData) ? moviesData : []);
      } catch (error) {
        console.error("Error fetching movies:", error);
        setMovies([]);
      }
    };

    fetchMovies();
    return () => controller.abort();
  }, []);

  return (
    <Layout className={`layout ${darkMode ? "dark" : "light"}`}>
      <Content className="content">
        <Title ><div className="title"><span>Phim đang chiếu</span> </div></Title>
        <div className="movie-carousel">
          {movies.length > 0 ? (
            <Carousel
              dots={true}
              autoplay
              autoplaySpeed={3000}
              slidesToShow={5} // Số phim hiển thị trên desktop
              slidesToScroll={1}
              responsive={[
                { breakpoint: 1024, settings: { slidesToShow: 3, slidesToScroll: 1 } },
                { breakpoint: 768, settings: { slidesToShow: 2, slidesToScroll: 1 } },
                { breakpoint: 480, settings: { slidesToShow: 1, slidesToScroll: 1 } },
              ]}
            >
              {movies.map((movie, index) => (
                <MovieCard key={movie.movieId || index} movie={movie} index={index} />
              ))}
            </Carousel>
          ) : (
            <p className="no-movie">Không có phim nào để hiển thị.</p>
          )}
          <div className="btn-see-more"><Button>Xem thêm</Button></div>

        </div>
      </Content>
      <Content className="content">
        <Title ><div className="title"><span>Phim sắp chiếu</span> </div></Title>
        <div className="movie-carousel">
          {movies.length > 0 ? (
            <Carousel
              dots={true}
              autoplay
              autoplaySpeed={3000}
              slidesToShow={5} // Số phim hiển thị trên desktop
              slidesToScroll={1}
              responsive={[
                { breakpoint: 1024, settings: { slidesToShow: 3, slidesToScroll: 1 } },
                { breakpoint: 768, settings: { slidesToShow: 2, slidesToScroll: 1 } },
                { breakpoint: 480, settings: { slidesToShow: 1, slidesToScroll: 1 } },
              ]}
            >
              {movies.map((movie, index) => (
                <MovieCard key={movie.movieId || index} movie={movie} index={index} />
              ))}
            </Carousel>
          ) : (
            <p className="no-movie">Không có phim nào để hiển thị.</p>
          )}
          <div className="btn-see-more"><Button>Xem thêm</Button></div>
        </div>
      </Content>

    </Layout>
  );
};

const MovieCard = ({ movie, index }) => {
  return (
    <motion.div
      className="movie-card"
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5, delay: index * 0.1 }}
    >
      <div className="movie-image">
        <Link to={`/movie/${movie.movieId}`}>
          <img src={movie.posterUrl || "https://via.placeholder.com/240x350"} alt={movie.title} />
        </Link>
      </div>
      <div className="movie-info">
        <p className="movie-rank"><span>{index + 1}</span></p>
        <p className="movie-title">{movie.title}</p>
        <p className="movie-genre">{movie.genre}</p>
        <p className="movie-description">{movie.description}</p>
        <p className="movie-rating"><Rate disabled defaultValue={movie.rating ? movie.rating : 4} /> </p>
      </div>
    </motion.div>
  );
};

export default HomePage;

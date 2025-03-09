import React, { useEffect, useState } from "react";
import { useSelector, useDispatch } from "react-redux";
import { Layout, Switch, Typography, Dropdown, Space ,Rate,Carousel} from "antd";
import { MoonOutlined, SunOutlined, DownOutlined } from "@ant-design/icons";
import { motion } from "framer-motion";
import MovieService from "../services/MovieService";
import Category from "../services/CategoryService"
import { toggleDarkMode } from "../store/themeSlice";

const { Header, Content, Footer } = Layout;
const { Title } = Typography;

const HomePage = () => {
  const [movies, setMovies] = useState([]);
  const [categorories, setCategories] = useState([]);
  const darkMode = useSelector((state) => state.theme.darkMode);
  const dispatch = useDispatch();

  useEffect(() => {
    const controller = new AbortController();
    const fetchMovies = async () => {
      try {
        const moviesData = await MovieService.searchMovies(controller.signal);
        setMovies(Array.isArray(moviesData) ? moviesData : []);
      } catch (error) {
        console.error("Error fetching movies:", error);
        setMovies([]); // Nếu lỗi thì đặt danh sách rỗng
      }
    };

    fetchMovies();
    return () => controller.abort();
  }, []);


  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const categorories = await Category.getAllCategory();
        console.log("API response:", categorories);
        setCategories(Array.isArray(categorories) ? categorories : []);
      } catch (error) {
        console.error("Error fetching category:", error)
        setCategories([]);
      }
    };
    fetchCategories();
  }, []);

  const items = categorories.map((category) => ({
    key: category.categoryId,
    label: category.categoryName
  }));
  return (
    <Layout className={`layout ${darkMode ? "dark" : "light"}`}>
      <Header className="header">
        <Title level={3} className="header-title"><span>TA-Cinema</span></Title>
        <Dropdown menu={{ items }}>
          <a>
            <Space className="dropdown">
             <span>Thể loại</span> 
              <DownOutlined />
            </Space>
          </a>

        </Dropdown>
        <Switch
          checked={darkMode}
          checkedChildren={<MoonOutlined />}
          unCheckedChildren={<SunOutlined />}
          onChange={() => dispatch(toggleDarkMode())}
        />
      </Header>

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
    </div>
      </Content>

      <Footer className="footer">CineFuture ©2025 - Experience the Future of Cinema</Footer>
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
        <img src={movie.posterUrl || "https://via.placeholder.com/240x350"} alt={movie.title} />
      </div>
      <div className="movie-info">
        <p className="movie-rank"><span>{index + 1}</span></p>
        <p className="movie-title">{movie.title}</p>
        <p className="movie-genre">{movie.genre}</p>
        <p className="movie-description">{movie.description}</p>
        <p className="movie-rating"><Rate disabled defaultValue={movie.rating ? movie.rating : 4}/> </p>
      </div>
    </motion.div>
  );
};

export default HomePage;

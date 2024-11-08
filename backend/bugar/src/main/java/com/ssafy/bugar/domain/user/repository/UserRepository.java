package com.ssafy.bugar.domain.user.repository;

import com.ssafy.bugar.domain.user.entity.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface UserRepository extends JpaRepository<User, Long> {
    User findByDeviceId(String deviceId);

    User findByUserId(Long userId);
}

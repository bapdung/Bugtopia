package com.ssafy.bugar.domain.insect.repository;

import com.ssafy.bugar.domain.insect.entity.Event;
import com.ssafy.bugar.domain.insect.enums.EventType;
import java.util.List;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

@Repository
public interface EventRepository extends JpaRepository<Event, Long> {

    Event findByEventId(Long eventId);

    Event findByEventName(EventType eventName);

    @Query("""
        SELECT e
        FROM Event e
        WHERE e.eventScore > :completedEventScore
        ORDER BY e.eventScore
    """)
    List<Event> getNotCompletedEvents(@Param("completedEventScore") int completedEventScore);

}
